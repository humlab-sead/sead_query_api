

test:
	dotnet test sead.query.test/sead.query.test.csproj

restore:
	dotnet restore

build:
	dotnet build

devdb:
	-source ./create_dev_db.sh --create-database --create-users
	
.PHONY test restore build

