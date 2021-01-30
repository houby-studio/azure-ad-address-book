# Azure AD Address Book

Browse all Azure AD contacts in simple HTML page. 

> No lazy loading. No heavy and messy UI.  
> Only data in a table.  

## Deployment

### Register Application in Azure

- Open portal.azure.com
- Click on **Azure Active Directory**
- Click on **App registrations**
- Click on **New registration**
- Choose **Name**
- Choose Supported account types **Accounts in this organizational directory only (COMPANY only - Single tenant)**
- Choose redirect URI - Web - **https://your.website/signin-oidc**
- Click on **Register**
- Click on **Authentication**
- Enable **ID tokens (used for implicit and hybrid flows)
Supported account types**
- Click on **API permissions**
- Click on **Add a permission**
- Click on **Microsoft Graph**
- Click on **Delegated**
- Click on **Delegated**
- Find and select **User.Read.All**
- Click on **Add permissions**
- Click on **Grant admin consent for COMPANY**
- Click on **Yes**
- Click on **Certificates & secrets**
- Click on **New client secret**
- Choose **Description**
- Choose **Expires** value
- Click on **Add**
- Copy secret value

Now you need to insert **Application ID** and **Client secret** along other tenant information to your deployment below.

### Deploy with docker-compose

```docker
version: "3.8"
services:
  azure-ad-address-book:
    container_name: azure-ad-address-book
    environment:
      - AZUREAD__DOMAIN=replacewithdomain.onmicrosoft.com
      - AZUREAD__CLIENTSECRET=replacewithclientsecret
      - AZUREAD__CLIENTID=replacewithclientid
      - AZUREAD__TENANTID=replacewithtenantid
      - COMPANYNAME=replacewithcompanyname
    restart: unless-stopped
    image: megastary/azure-ad-address-book
    ports:
      - 80:80
      - 443:443
```

### Deploy with docker

```bash
docker run \
-p 80:80 -p 443:443 \
-e AZUREAD__DOMAIN=replacewithdomain.onmicrosoft.com \
-e AZUREAD__CLIENTSECRET=replacewithclientsecret \
-e AZUREAD__CLIENTID=replacewithclientid \
-e AZUREAD__TENANTID=replacewithtenantid \
-e COMPANYNAME=replacewithcompanyname \
--restart=unless-stopped \
megastary/azure-ad-address-book
```

## Developers

This application is based on ASP.NET MVC Core Web app signing-in users with the Microsoft identity platform in your organization.  
It is basically a tutorial application slightly transformed to provide real world use.  
But after all, it is just a step on our path to learning ASP.NET and other technologies around it.

### Resources

[Tutorials map](https://raw.githubusercontent.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2/master/ReadmeFiles/aspnetcore-webapp-tutorial-alt.svg)

[Reference repository](https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2/tree/master/1-WebApp-OIDC/1-1-MyOrg)

[How to make ASP.NET MVC app work behind reverse proxy](https://stackoverflow.com/questions/59354864/azure-ad-authentication-redirect-uri-not-using-https-on-linux-hosted-cloud-foun)

[How to make ASP.NET MVC app work behind reverse proxy](https://stackoverflow.com/questions/49189883/how-to-set-redirect-uri-protocol-to-https-in-azure-web-apps?rq=1)

[How to sign out](https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2/tree/master/1-WebApp-OIDC/1-6-SignOut)
