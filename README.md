# Tridion 2013 YouTube ECL Provider with google youtube v3 #

This is an External Content Library provider which exposes videos from YouTube in SDL Tridion 2013. The first version supports displaying videos from a single specified account, but could potentially be extended to allow search, multiple users, video upload and publishing in future.

# Updates #
Supports multiple users/channels
Supports proxy based access to youtube thumbnails

# Need Google Api Key to be generated -OAuth 2.0 #

Please use below links to generate it

https://developers.google.com/console/help/new/#generatingdevkeys
https://console.developers.google.com/

# Sample ExternalContentLibrary.xml #
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Configuration xmlns="http://www.sdltridion.com/ExternalContentLibrary/Configuration">
    <!-- Available logging levels: Debug, Info, Warning, Error -->
    <l:Logging Level="Warning" xmlns:l="http://www.sdltridion.com/Infrastructure/LoggingConfiguration">
        <!-- Additional supported attributes: Language="rfc1766 code" Locale="rfc1766 code". An example of an rfc1766 code is "de-DE" for German. /-->
        <!-- <l:Folder>Full path to logging folder, default is %TRIDION%/bin/log</l:Folder>-->
    </l:Logging>
    <CoreServiceUrl>net.tcp://localhost:2660/CoreService/2011/netTcp</CoreServiceUrl>
    <MountPoints>
        <MountPoint type="YouTubeProvider" version="*" id="youtube" rootItemName="YouTube">
            <StubFolders>
                <StubFolder id="tcm:1-3-2" />
            </StubFolders>
            <PrivilegedUserName>DOMAIN\user</PrivilegedUserName>
            <AppName xmlns="http://gdata.youtube.com/schemas/2007">Your YouTube App Name</AppName>
            <ApiKey xmlns="http://gdata.youtube.com/schemas/2007">Your YouTube Developer Key</ApiKey>
            <Username xmlns="http://gdata.youtube.com/schemas/2007">Your YouTube/Google Username</Username>
            <Users xmlns="http://gdata.youtube.com/schemas/2007">
                <User></User>
                <User></User>
            </Users>
            <ProxyURI xmlns="http://gdata.youtube.com/schemas/2007">Proxy Address</ProxyURI>
            <ProxyUser xmlns="http://gdata.youtube.com/schemas/2007">Proxy Username </ProxyUser>
            <ProxyPassword xmlns="http://gdata.youtube.com/schemas/2007">Proxy Password</ProxyPassword>
        </MountPoint>
    </MountPoints>
</Configuration>
