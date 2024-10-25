@pushd %~dp0
@cd src\ResourceFileServer\
start /MIN dotnet run
@popd

@pushd %~dp0
@cd src\ResourceServer\
start /MIN dotnet run
@popd

@pushd %~dp0

SET DOTNET_WATCH_SUPPRESS_LAUNCH_BROWSER=1
@cd src\AngularClientImplicitFlow\
start /MIN npm run build & npm start
@popd

