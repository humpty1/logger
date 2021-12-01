rem SET R=/r:Logger.cs.dll
rem SET R=/r:ClassLibrary1.dll 
rem     public enum IMPORTANCELEVEL { Spam, Debug, Warning, Stats, Error, FatalError, Info,  Ignore };
SET R=/r:Logger.cs.dll /r:args.dll 
echo off

SET NAMEZIP=logger
echo %NAMEZIP%
SET RM= _bld  foo/*.exe dll_usage/*.exe 
rem SET EXZIP=-x .* -x *.eRr -x *.exe -x *.log
