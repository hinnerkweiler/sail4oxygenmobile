#sail4oxygen

### 0.9 (Android only)

+ changed mime type for share intent from text/comma-seperated-values to */*

+ Validates given file as KOR generated csv file and that we can work with it (ID name, Headers in line 10 and comma as seperator):
    ```(fileContent[1].StartsWith("Kor MEASUREMENT DATA FILE EXPORT") && 
                    fileContent[10].StartsWith("Date (MM/DD/YYYY)") && 
                    (fileContent[10].IndexOf(',') > -1))```


+ Reflect FileName and selection state in UI

+ Validate Coordinates as decimals between 54N-56N and 009E-011E
    blocking send button if
     + invalid value range
     + invalid characters

+ Ask if Coordinates have been adjusted accordingly if first measurement in CSV is older than 30 Minutes (~4nm from Spot)

+ Feedback if file could not be read or is of wrong format

+ Feedback after sending

+ various Errormessages in the workflow as PopUpboxes

+ Cleanup after a file has been sent

+ Manually cleanup in case wrong file was manually selected

+ Share Intent full rewrite. (Platform dependent code not implemented for iOS and other OSs)

+ Removed FAQ/Guide Page


### 0.8
+ allow CSV to be shared from KOR App

### 0.7
+ Beta 1

### < 0.6
+ Functional Prototype for internal testing

