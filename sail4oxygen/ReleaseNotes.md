﻿#sail4oxygen

## 0.12.2-rc1 (Android only)

+ UX: better visuals with brighter colors when using dark mode
+ UX: white stancil image in dark mode
+ UX: Display a message when sending data to select the e-mail app if share-sheet is displayed
+ bugfix: fixing manually entered coordinates in wired combinations (e.g. en-DE) and force writing coordintes in en-US culture to CSV
+ bugfix: detect and abort if CSV has been opened/saved with other apps that have altered measurement data to phone's locale (e.g. 9.12 -> "9,12")


## 0.12.1 (Android only)

+ fixing an issue where no warnig is displayed when coordinates are invalid.

+ adding a field to enter a boat name in the preferences screen

+ enlarging the legally allowed coordinates 

## 0.12.0 (Android only)

+ added latest Manual as PDF

## 0.11.1 (Android only)

+ Register for MimeType */* as KOR ssem to not export text/csv correctly

## 0.11.0 (Android only)
### Public Beta – Please join on Google Play-Store!

+ minor fixes and code cleanup

+ adding RSS Reader to display latest sail4oxygen news

+ visuals

+ (Dependency Newtonsoft.Json added)

### 0.10.2 (Android only)

+ change share intent to only respond to MIME Type "text/csv"


### 0.10 (Android only)

+ Validate measurement age againts UTC

+ Translations base file and defaults for englisch

+ Informed consent required: App will ask for consent to send mails revealing email address and location to geomar.  If this consent is not given it will prevent the user from accessing the sending files. This way we sould net be required to maintain a record of consent.

+ Privacy note on About page with privacy contact: *datenschutz@geomar.de* as the app does no sort of user tracking by itself for now

+ Translation for German added



### 0.9.1 (Android only)

+ allow dot notation for decimal coordinates



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
+ Functional Prototype for internal testing##Components usewd###SkiaSharpCopyright (c) 2015-2016 Xamarin, Inc.Copyright (c) 2017-2018 Microsoft Corporation.Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.