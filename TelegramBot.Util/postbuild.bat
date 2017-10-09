echo %TargetDir%*.*
pushd %TelegramPythonUtilDir%
mkdir "bin"
popd
echo copy "%TargetDir%*.*" "%TelegramPythonUtilDir%bin\"
copy "%TargetDir%*.*" "%TelegramPythonUtilDir%bin\"
exit /b 0