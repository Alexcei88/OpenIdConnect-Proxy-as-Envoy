@pushd %~dp0
@cd ResourceFileServer\
start /MIN dotnet run
@popd

@pushd %~dp0
@cd ResourceServer\
start /MIN dotnet run
@popd
