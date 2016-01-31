@echo off
for %%f in (*.png) do (
    echo Processing: %%~nf
    xxd -i "%%~nf.png" > "%%~nf.h"
)
pause