# Sample of Simple JSON Grafana Rest interface

This connector used [SimpleJSON](https://grafana.com/grafana/plugins/grafana-simple-json-datasource/) Data Source of Grafana. It connects to REST Web Service.

## Install Grafana

See [here](https://grafana.com/docs/grafana/latest/setup-grafana/installation/) for further details to install Grafana (OSS). 

* *Windows*
  - Use the [Windows Installer](https://grafana.com/grafana/download?edition=oss&pg=get&platform=windows&plcmt=selfmanaged-box1-cta1) which describes the Windows installer package or a standalone Windows binary file.

  or alternatively you can use a package manager for your installation process 
  - [Install chocolatey](https://chocolatey.org/install) 
  - Run in shell (with admin rights): ``choco install grafana``
  
* *Linux* 
   - Follow Grafanas [Install Documentation](https://grafana.com/docs/grafana/latest/setup-grafana/installation/) which explains how to install Grafana dependencies, download and install Grafana, get the service up and running on your Linux system, and also describes the installation package details.

   or if you prefer to communicate via Reverse Proxy with the Grafana server in your Ubuntu 20.04
   -  Follow [How to Install Grafana on Ubuntu 20.04](https://linuxhostsupport.com/blog/how-to-install-grafana-on-ubuntu-20-04/)

* *MacOS*
  - [MacOS](https://grafana.com/docs/grafana/latest/setup-grafana/installation/mac/) explains how to install Grafana and get the service running on your macOS.

After installation start and check your Grafana instance via http://localhost:3000, username ``admin`` and password ``admin``. 

## Prepare Grafana 

In Grafana we need a generic JSON backend datasource for our project. Before we install the SimpleJSON PlugIn, download the Asset Administration Shell Grafana Connector Sample Project and start the Demo Application. Check if the page https://localhost:7240/swagger/index.html (Swagger Page) is available.

Install the SimpleJSON Plugin via [Plug-Ins page](http://localhost:3000/plugins?filterBy=all&filterByType=all&q=JSON) now. After installation click the button [create a SimpleJSON Data source] to define a new data connection between Grafana and the demo application.      
Enter URI (e. g. http://localhost:5028/) in section HTTP and give the data source a name.

Create a dashboard and set a dashboard variable [see docs.](https://grafana.com/docs/grafana/latest/dashboards/variables/)

Select your previously created data source and give the variable a name (e. g. 'tag').

Create a dashboard element and goto settings. Select your data source again.

Enter on first query (marked as 'A'), the variable name - if you have called the variable 'tag' use $tag to reference to the value of the variable.




