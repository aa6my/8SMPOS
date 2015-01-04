# 8SMPOS

Reference for distributing appx VS12. 

  - http://stackoverflow.com/questions/12512196/how-to-install-a-windows-8-app-without-submitting-to-store
  - http://stackoverflow.com/questions/23812471/installing-appx-without-trusted-certificate

Here the step to distributing appx :

#### <i class="icon-hdd"></i> Creating Certificate
```
Package.appxmanifest > Packaging > Choose Certifcate > Create New		
```
	
#### <i class="icon-hdd"></i> Create Package appx
```
Project > Store > Create App Packages > No & Next > Create		
```

#### <i class="icon-hdd"></i> Import certficate		
```
- Double-tap the certificate file in the folder and then tap Install Certificate. This displays the Certificate Import Wizard.		
- In the Store Location group, tap the radio button to change the selected option to Local Machine.		
- Click Next. Tap OK to confirm the UAC dialog.		
- In the next screen of the Certificate Import Wizard, change the selected option to Place all certificates in the following store.		
- Tap the Browse button. In the Select Certificate Store pop-up window, scroll down and select Trusted People, and then tap OK.		
- Tap the Next button; a new screen appears. Tap the Finish button.		
- A confirmation dialog should appear; if so, click OK. (If a different dialog indicates that there is some problem with the certificate, you may need to do some certificate troubleshooting. However, describing what to do in that case is beyond the scope of this topic.)		
```

#### <i class="icon-hdd"></i> Install using powershell
```
add-appxpackage pos.appx
```

### Version
Beta

### Tech

8SMPOS uses a number of open source projects & lisence software to work properly:

* [AngularJS] - HTML enhanced for web apps!
* [ionic] - awesome web-based text editor
* [node.js] - evented I/O for the backend
* [jQuery] - duh
* [Visual Studio] - You eating my money.

License
----

SeGi Midae All Right Reserved


**Paid Software**

[ionic]:http://ionicframework.com/
[node.js]:http://nodejs.org
[jQuery]:http://jquery.com
[AngularJS]:http://angularjs.org
[Visual Studio]:http://www.visualstudio.com/
