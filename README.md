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
Actually, the main purpose of this project was to practice `TDD` style with `DI` patterns and I’ve fell in love with them. Trying to write in the test-first style is so natural; I don’t know how I live without it before. However, it is not always obvious how to write simple, readable and maintainable tests.  With `DI` I’ve tried to follow canonical approach from [Mark Seemann’s book](http://www.amazon.com/Dependency-Injection-NET-Mark-Seemann/dp/1935182501): using `DI` container only in the `Composition Root`, resolve run-time dependencies through `Abstract Factories` implemented in the `Composition Root` too and even a little dose of `Interception`. Reading his ultra-useful [blog](http://blog.ploeh.dk/) gave even more knowledges. 

Also Finances gave a huge practice in `WPF, SOLID, MVVM, LINQ, GIT` and even `R#`, which I love too. There are many things needs to implement them better, for example, better tests coverage, more strictly design in `ViewModel-First` style, uniform way to work with `WPF` Templates and so on. I hope to finish it and gain my skills far more.
 


###Screenshots:
![1](https://cloud.githubusercontent.com/assets/5301844/11852649/e10a77a2-a441-11e5-90f5-ed2bc44ec2f3.png)
![2](https://cloud.githubusercontent.com/assets/5301844/11852650/e11124b2-a441-11e5-9ca0-2a9fe4668a4b.png)
![3](https://cloud.githubusercontent.com/assets/5301844/11852651/e12272c6-a441-11e5-8902-940df6fd7170.png)
![4](https://cloud.githubusercontent.com/assets/5301844/11852652/e122fe3a-a441-11e5-8178-4a7eb61c3676.png)
![5](https://cloud.githubusercontent.com/assets/5301844/11852653/e1250e32-a441-11e5-98c4-7956a0a38844.png)
