.PHONY: test coverage build clean

test:
	dotnet test

coverage:
	dotnet test engine/ConflictVessels.Engine.Tests -p:CollectCoverage=true -p:CoverletOutputFormat=teamcity

build:
	dotnet build

clean:
	dotnet clean
