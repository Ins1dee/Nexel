
# Nexel

Welcome to the Nexel - Excel-Like App! This application provides you with the simple calculator that allows you to perform math operations ( +, -, /, *, ^ and () ) and even some functions (sin, cos, tan, sqrt, log, exp, abs).


## Run Project

To deploy this project run this command in directory where docker compose file is

```bash
    docker-compose up --build
```


## Run Tests

To start tests for this project run this command in directory where docker compose file is

```bash
    docker-compose -f docker-compose.yml run --rm unit_tests
```

## My thoughts
I recently got acquainted with Clean Architecture and DDD, so i used them in my project. It was a good experience to practice them, as well as docker which i never used before. As for next steps to make this service better, i guess it would be great to continue develop such a Exel-Like app, adding more functionality that are available in Exel. 
