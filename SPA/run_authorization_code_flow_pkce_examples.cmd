@pushd %~dp0
@cd src\ResourceFileServer\
start /MIN dotnet run
@popd

@pushd %~dp0
@cd src\ResourceServer\
start /MIN dotnet run
@popd

@pushd %~dp0
@cd src\AngularClientCode\
start /MIN npm run build & npm start
@popd
