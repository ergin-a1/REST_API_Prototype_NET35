1- Add rewrite IIS module to get rid of SVC extension from REST service and add the follwoing code to your web.config file in between the <system.webServer> </system.webServer> section.

		<rewrite>
            <rewriteMaps>
                <rewriteMap name="/APIPrototypeAPI" />
            </rewriteMaps>
            <rules>
                <rule name="ClearSVCExtension" stopProcessing="false">
                    <match url="^([0-9a-zA-Z\-]+)/([0-9]).([0-9])/([0-9a-zA-Z\-\.\/\(\)]+)" />
                    <action type="Rewrite" url="{R:1}.svc/{R:2}{R:3}/{R:4}" />
                </rule>
            </rules>
        </rewrite>

2- In order to make your Response type dynamic, please add Factory class to your *.svc file as show below 

<%@ ServiceHost Language="C#" Debug="true" Service="APIPrototype.APIPrototypeService" CodeBehind="APIPrototypeService.svc.cs" Factory="DamianBlog.ServiceHostFactory2Ex" %>

