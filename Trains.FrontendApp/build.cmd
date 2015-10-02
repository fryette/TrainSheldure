pushd "%~dp0"

call npm install
call node_modules\.bin\bower.cmd install
call node_modules\.bin\gulp.cmd

popd
