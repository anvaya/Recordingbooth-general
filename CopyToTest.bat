rem Copy files to test directory
rem
rem Delete old files
del \test\RecordingBooth\*.*
rem
rem Copy new files
copy RecordingBooth\bin\Debug\app.config \test\RecordingBooth\app.config
copy RecordingBooth\bin\Debug\AudioRecorder.dll \test\RecordingBooth\AudioRecorder.dll
copy RecordingBooth\bin\Debug\NAudio.dll \test\RecordingBooth\NAudio.dll
copy RecordingBooth\bin\Debug\NAudio.xml \test\RecordingBooth\NAudio.xml
copy RecordingBooth\bin\Debug\NLog.config \test\RecordingBooth\NLog.config
copy RecordingBooth\bin\Debug\NLog.dll \test\RecordingBooth\NLog.dll
copy RecordingBooth\bin\Debug\NLog.xml \test\RecordingBooth\NLog.xml
copy RecordingBooth\bin\Debug\RecordingBooth.exe \test\RecordingBooth\RecordingBooth.exe
copy RecordingBooth\bin\Debug\RecordingBooth.exe.config \test\RecordingBooth\RecordingBooth.exe.config
copy RecordingBooth\bin\Debug\RecordingLibrary.dll \test\RecordingBooth\RecordingLibrary.dll
copy RecordingBooth\bin\Debug\RecordingsData.sdf \test\RecordingBooth\RecordingsData.sdf
copy RecordingBooth\bin\Debug\sqlceca35.dll \test\RecordingBooth\sqlceca35.dll
copy RecordingBooth\bin\Debug\sqlcecompact35.dll \test\RecordingBooth\sqlcecompact35.dll
copy RecordingBooth\bin\Debug\sqlceer35EN.dll \test\RecordingBooth\sqlceer35EN.dll
copy RecordingBooth\bin\Debug\sqlceme35.dll \test\RecordingBooth\sqlceme35.dll
copy RecordingBooth\bin\Debug\sqlceoledb35.dll \test\RecordingBooth\sqlceoledb35.dll
copy RecordingBooth\bin\Debug\sqlceqp35.dll \test\RecordingBooth\sqlceqp35.dll
copy RecordingBooth\bin\Debug\sqlcese35.dll \test\RecordingBooth\sqlcese35.dll
copy RecordingBooth\bin\Debug\System.Data.SqlServerCe.dll \test\RecordingBooth\System.Data.SqlServerCe.dll
