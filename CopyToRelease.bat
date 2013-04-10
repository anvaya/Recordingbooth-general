rem Copy files to test directory
rem
rem Delete old files
del \release\RecordingBooth\*.*
rem
rem Copy new files
copy RecordingBooth\bin\Release\app.config \release\RecordingBooth\app.config
copy RecordingBooth\bin\Release\AudioRecorder.dll \release\RecordingBooth\AudioRecorder.dll
copy RecordingBooth\bin\Release\NAudio.dll \release\RecordingBooth\NAudio.dll
copy RecordingBooth\bin\Release\NAudio.xml \release\RecordingBooth\NAudio.xml
copy RecordingBooth\bin\Release\NLog.config \release\RecordingBooth\NLog.config
copy RecordingBooth\bin\Release\NLog.dll \release\RecordingBooth\NLog.dll
copy RecordingBooth\bin\Release\NLog.xml \release\RecordingBooth\NLog.xml
copy RecordingBooth\bin\Release\RecordingBooth.exe \release\RecordingBooth\RecordingBooth.exe
copy RecordingBooth\bin\Release\RecordingBooth.exe.config \release\RecordingBooth\RecordingBooth.exe.config
copy RecordingBooth\bin\Release\RecordingLibrary.dll \release\RecordingBooth\RecordingLibrary.dll
copy RecordingBooth\bin\Release\RecordingsData.sdf \release\RecordingBooth\RecordingsData.sdf
copy RecordingBooth\bin\Release\sqlceca35.dll \release\RecordingBooth\sqlceca35.dll
copy RecordingBooth\bin\Release\sqlcecompact35.dll \release\RecordingBooth\sqlcecompact35.dll
copy RecordingBooth\bin\Release\sqlceer35EN.dll \release\RecordingBooth\sqlceer35EN.dll
copy RecordingBooth\bin\Release\sqlceme35.dll \release\RecordingBooth\sqlceme35.dll
copy RecordingBooth\bin\Release\sqlceoledb35.dll \release\RecordingBooth\sqlceoledb35.dll
copy RecordingBooth\bin\Release\sqlceqp35.dll \release\RecordingBooth\sqlceqp35.dll
copy RecordingBooth\bin\Release\sqlcese35.dll \release\RecordingBooth\sqlcese35.dll
copy RecordingBooth\bin\Release\System.Data.SqlServerCe.dll \release\RecordingBooth\System.Data.SqlServerCe.dll
