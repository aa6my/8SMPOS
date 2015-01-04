POS Reference
=============

- http://stackoverflow.com/questions/12512196/how-to-install-a-windows-8-app-without-submitting-to-store
- http://stackoverflow.com/questions/23812471/installing-appx-without-trusted-certificate

- 1st Step
Package.appxmanifest > Packaging > Choose Certifcate > Create New

- 2nd Step
Project > Store > Create App Packages > No & Next > Create

- 3rd Step
* Double-tap the certificate file in the folder and then tap Install Certificate. This displays the Certificate Import Wizard.
* In the Store Location group, tap the radio button to change the selected option to Local Machine.
* Click Next. Tap OK to confirm the UAC dialog.
* In the next screen of the Certificate Import Wizard, change the selected option to Place all certificates in the following store.
* Tap the Browse button. In the Select Certificate Store pop-up window, scroll down and select Trusted People, and then tap OK.
* Tap the Next button; a new screen appears. Tap the Finish button.
* A confirmation dialog should appear; if so, click OK. (If a different dialog indicates that there is some problem with the certificate, you may need to do some certificate troubleshooting. However, describing what to do in that case is beyond the scope of this topic.)

-4th Step
powershell
add-appxpackage pos.appx


