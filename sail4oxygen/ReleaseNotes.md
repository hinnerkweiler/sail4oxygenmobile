#sail4oxygen

### 0.9

* Validates Coordinates as decimal numbers between 54N-56N 009E-011E

* Validates given file as KOR generated:
    ```(fileContent[1].StartsWith("Kor MEASUREMENT DATA FILE EXPORT") && 
                    fileContent[headerRowNumber].StartsWith("Date (MM/DD/YYYY)") && 
                    (fileContent[headerRowNumber].IndexOf(',') > -1))```

* Warn to adujst Coordinates if first measurement is older than 45 Minutes (~5nm from Spot)

* added various User Feedback, Errormessages and 


### 0.8
* allow CSV to be shared from KOR App

### 0.7
* Beta 1

### < 0.6
* Internal Development

