#sail4oxygen ReadMe

*Great projects often start with a terrible underestimation: "We might need kind of a registration page at one point. Probably a Google-Form would do it", was how I came into the Citizen Science: Sailing for Oxygen Project and here am I writing the Readme for a mobile app…*

For more information about Citizen Science: Sailing for Oxygen, the project behind this app, go to [sail4oxygen.org](https://sail4oxygen.org) (german) or see the [official press release in english](https://www.geomar.de/en/news/article/fishing-data-for-science)

##Release Status:
Beta Testing - (Please get in contact if you want to help with testing and development)

##Release Notes
[here](sail4oxygen/ReleaseNotes.md)

##Using the app 
The main purpose is to receive a shared CSV-File from that previously mentioned KOR App with a measurement in it. Multiple measurements in one file will also work, as long as they are taken as a time series at the some location. The reson for that limitation is the design of the exported CSV File and the fact, that it does not provide location data. sail4oxygen jumps in at that point and adds the either automatically or manually entered GPS-Latitude and Longitude. 
Once done, sail4oxygen will pack everything and send it via email to GEOMAR where your measurement will be processed and published. 

###Steps 

-Do a measurement with the sonde 
-Download the Data using the KOR-App 
-Export Data as CSV File 
-Select GEOMAR from the list of Share Targets 
-(correct the GPS Coordinates if needed) 
-Press “Send to GEOMAR” 
-A Mail Message will be displayed to be sent.

##Credits
Like most software today this project heavily depended on people willing to share their good knowledge on Stackoverflow. That said, I have to admit that my first asking more and more often goes to Github Copilot and ChatGPT. 

###LIBRARIES USED: 
**CommunityToolkit.Mvvm (version 8.2.1)** The CommunityToolkit.Mvvm is a powerful open-source library developed by the community under the MIT License. It provides a set of MVVM (Model-View-ViewModel) components and utilities for building cross-platform applications. The CommunityToolkit.Mvvm simplifies the implementation of the MVVM pattern, enabling developers to write maintainable and testable code. 

**CommunityToolkit.Maui (version 5.2.0):** The CommunityToolkit.Maui is an open-source library developed by the community, providing a collection of controls, helpers, and services for building cross-platform applications using Microsoft MAUI. It offers a range of UI components and utilities that enhance the development experience and enable the creation of visually appealing and functional user interfaces. The Librariy is also released under MIT License.
