Connecting to 3rd party APIs

To run, open in visual studio 2022 - set startup projects to BlazorConnectToAPI and PropertyAPI 

Bazor Project connects to local API

Local API connects to public API to validate postcode and get land registry information

e.g. Postcode validation on RH10 3EE - https://postcodes.io/postcodes/RH10%203EE

validated postcode is then used for a call to land registry

e.g. https://landregistry.data.gov.uk/app/ppd/ppd_data.csv?postcode=RH10+3EE

results sent back to blazor app
