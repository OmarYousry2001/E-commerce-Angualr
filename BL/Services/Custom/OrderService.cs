
using BL.Abstracts;
using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.IMapper;
using BL.Contracts.Services.Custom;
using BL.DTO.Entities;
using BL.GenericResponse;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Product;
using Domains.Order;
using Microsoft.EntityFrameworkCore;
namespace BL.Services
{
    public class OrderService : BaseService<Orders, OrderDTO>, IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerBasketService _customerBasketService;
        private readonly IPaymentService _paymentService;

        
        public OrderService(
            ITableRepository<Orders> orderTableRepository,
            IBaseMapper mapper,
            IUnitOfWork unitOfWork,
            ICustomerBasketService customerBasketService,
            IPaymentService paymentService
              )
            : base(unitOfWork.TableRepository<Orders>(), mapper)
        {
            _unitOfWork = unitOfWork;
            _customerBasketService = customerBasketService;
            _paymentService = paymentService;



        }


        public async Task<Response<OrderDTO>> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail, Guid userId)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var basketResponse = await _customerBasketService.GetBasketAsync(orderDTO.BasketId);

                List<OrderItem> orderItems = new List<OrderItem>();

                // Full OrderItem By BasketItems
                foreach (var item in basketResponse.Data.BasketItems)
                {

                    var Product = await _unitOfWork.TableRepository<Product>().FindByIdAsync(item.Id);
                    var orderItem = new OrderItem
                        (Product.Id, item.Image, Product.Name, item.Price, item.Quantity, userId);
                    orderItems.Add(orderItem);

                }

                var deliverMethod = await _unitOfWork.TableRepository<DeliveryMethod>()
                    .GetTableAsTracking().Where(x => x.Id == orderDTO.DeliveryMethodId && x.CurrentState == 1).FirstOrDefaultAsync();

                var subTotal = orderItems.Sum(m => m.Price * m.Quantity);

                var ship = _mapper.MapModel<ShipAddressDTO, ShippingAddress>(orderDTO.ShipAddress);


                var ExistOrder = await _unitOfWork.TableRepository<Orders>().FindAsync(m => m.PaymentIntentId == basketResponse.Data.PaymentIntentId && m.CurrentState == 1);
                if (ExistOrder is not null)
                {
                    await _unitOfWork.TableRepository<Orders>().UpdateCurrentStateAsync(ExistOrder.Id, userId);
                    await _paymentService.CreateOrUpdatePaymentAsync(basketResponse.Data.PaymentIntentId, deliverMethod.Id);
                }

                var order = new
                         Orders(BuyerEmail, subTotal, ship, deliverMethod, orderItems, basketResponse.Data.PaymentIntentId);

                //order.PaymentIntentId = "omar1234";

                await _unitOfWork.TableRepository<Orders>().CreateAsync(order, userId);
                await _customerBasketService.DeleteBasketAsync(orderDTO.BasketId);

                await transaction.CommitAsync();
                orderDTO.Id = order.Id;
                return Success(orderDTO);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task<Response<IEnumerable<OrderToReturnDTO>>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var orders = await _unitOfWork.TableRepository<Orders>()
                .GetAsync(x => x.CurrentState == 1 && x.BuyerEmail == BuyerEmail,
                x => x.orderItems,
                x => x.deliveryMethod);
            var result = _mapper.MapList<Orders, OrderToReturnDTO>(orders);

            result = result.OrderByDescending(m => m.Id).ToList();
            return Success(result);
        }

        public async Task<Response<OrderToReturnDTO>> GetOrderByIdAsync(Guid Id, string BuyerEmail)
        { 
            var order = await _unitOfWork.TableRepository<Orders>()
                        .FindAsync(x => x.CurrentState == 1 && x.Id == Id && x.BuyerEmail == BuyerEmail,
                        x => x.orderItems,
                        x => x.deliveryMethod);
            var result = _mapper.MapModel<Orders, OrderToReturnDTO>(order);
            return Success(result);
        }

        public async Task<Response<OrderToReturnDTO>> FindAsync(string PaymentIntentId)
        {
            var order = await _unitOfWork.TableRepository<Orders>()
                 .FindAsync(x => x.CurrentState == 1 && x.PaymentIntentId == PaymentIntentId);
            var result = _mapper.MapModel<Orders, OrderToReturnDTO>(order);
            return Success(result); ;
        }
        public async Task<Response<bool>> SaveStatusPaymentAsync(OrderToReturnDTO orderToReturnDTO, Guid userId)
        {
            var entity = _mapper.MapModel<OrderToReturnDTO, Orders>(orderToReturnDTO);
            var isSaved = await _unitOfWork.TableRepository<Orders>().SaveAsync(entity, userId);
            if (isSaved) return Success(true);
            else return BadRequest<bool>();
        }

    }
}
