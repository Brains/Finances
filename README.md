# Finances

It's personal finances tracker and analyzer. It perfectly fits for my own usage patterns. 

Under the hood, this project uses `Caliburn.Micro` framework and `Unity` as carcass constructions. `MahApps.Metro` toolkit and `ModernUICharts` library are there for visual appearance and charts building, but I want to replace the latter to the `WPF Toolkit Data Visualization` + Own styles. There is a lot of work to implement in future.

### Features:
- Tracking expenses, incomes, shared spending, debts.
- Build statistics such as spending by day, by category for month and year, expense to outcome comparison.
- Funds dashboard that shows all fund sources (*cash, credit card, debts*) together.
- Calculate divergence between real and estimated balance after each operation
- Pulls credit card balance and last transactions (*work in progress*) through online banking API.
- Builds visual prediction by each day about future money balance based on permanent operations (*rent, food, deposits, sport, utility bills*).

### Goals
Actually, the main purpose of this project was to practice `TDD` style with `DI` patterns and I’ve fell in love with them. Trying to write in the test-first style is so natural; I don’t know how I live without it before. However, it is not always obvious how to write simple, readable and maintainable tests.  With `DI` I’ve tried to follow canonical approach from [Mark Seemann’s book](http://www.amazon.com/Dependency-Injection-NET-Mark-Seemann/dp/1935182501): using `DI` container only in the `Composition Root`, resolve run-time dependencies through `Abstract Factories` implemented in the `Composition Root` too and even bring a little dose of `Interception`. Reading his ultra-useful [blog](http://blog.ploeh.dk/) gave even more knowledges. 

Also Finances gave a huge practice in `WPF, SOLID, MVVM, LINQ, GIT` and even `R#`, which I love too. There are many things needs to implement them better, for example, wider tests coverage, more strictly design in `ViewModel-First` style, uniform way to work with `WPF` Templates and so on. I hope to finish it soon and gain my skills far more.

###Screenshots:
![tracker](https://cloud.githubusercontent.com/assets/5301844/12006754/480f7c20-abec-11e5-8e58-f73d67e1c521.gif)
![statistics](https://cloud.githubusercontent.com/assets/5301844/12006799/5a1edb2a-abee-11e5-8e47-4ee2a960eb48.gif)
