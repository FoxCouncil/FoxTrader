@echo off
for %%f in (*.ttf) do (
    echo Processing: %%~nf
    xxd -i "%%~nf.ttf" > "%%~nf.h"
)
pause