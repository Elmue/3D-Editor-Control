
REM Cleanup all intermediate files from Visual Studio

attrib -h *.suo
del *.suo

move SourceCode\Release\Editor3D.exe  Editor3D.exe

rmdir SourceCode\obj /S /Q
rmdir SourceCode\Debug /S /Q
rmdir SourceCode\Release /S /Q
