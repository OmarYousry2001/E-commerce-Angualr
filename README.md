A production‑ready, clean‑architecture e‑commerce application built with ASP.NET Core Web API + Angular. It implements N‑Layer Architecture, Clean Code principles, and integrates Stripe for payments, Redis for caching, JWT for auth, Serilog for logging, image WebP conversion, Unit of Work, Generic Repository, and AutoMapper.

✨ Key Features

👤 Authentication & Authorization with JWT (access token) + role support

🛍️ Product catalogue, baskets, checkout flow

💳 Stripe Payments (Payment Intent + confirmCardPayment)

🖼️ Image uploads with automatic WebP conversion & optimized storage

🚀 N‑Layer + Clean Architecture (Domain, Application/BL, Infrastructure/DAL, API, UI)

🔁 Unit of Work + Generic Repository + AutoMapper mapping profiles

⚡ Redis caching (hot paths like products, delivery methods)

🧪 Unit Testing ready (services & core logic)

🧭 Structured logging via Serilog (Console + rolling file)


🛠️ Tech Stack

Backend: .NET 9, ASP.NET Core Web API, EF Core, AutoMapper, Serilog, Redis

Database: SQL Server (can adapt to PostgreSQL)

Frontend: Angular, RxJS, Toastr, Stripe.js Elements

Messaging/Cache: Redis (in‑memory distributed cache)

Auth: JWT (HMAC‑SHA256)

Payments: Stripe (Intent‑based)
