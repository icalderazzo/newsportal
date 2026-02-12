.PHONY: init up down clean test

init:
	dotnet restore
	dotnet build
	docker-compose --profile init up --build -d

up:
	docker compose up -d

down:
	docker compose down

clean:
	docker compose --profile "*" down -v --rmi all

test:
	dotnet test