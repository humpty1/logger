
echo on
call  g:/bin/etc/params.%COMPUTERNAME%.cmd
echo off


G:\bin\doxygen.1.8.9.1\doxygen.exe report.doxygen
cd "_bld/html"
ls *.hhp
G:\agp\bin\help\hhc.exe  index.hhp 
cd ..
cd ..
cp   ./_bld/html/report.chm .
rem cp refman.tex G:\agp\128\prj\system\logger\_bld\latex\
