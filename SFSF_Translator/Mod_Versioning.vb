Module Mod_Versioning

    'Ver 1.0
    'Fixed Issue with Copy/Paste shared events in CP Form
    'Removed tags from xml definition, as the SF system doesnt want them translated. FileNames: 5.2,5.3,5.4,5.13,6.1,6.2,6.5
    'Removed Tags
    '5.2
    '1. “template-desc “ & “description” tags are not allowed to be translated. 
    '2. Please also ensure the reimport logic consider the xml definition file, so that if a tag should be suppressed we need to do this ONLY in the definition and not in the code. I think this is rather urgent, because I suppose there will more tags in other files which will require suppression.
    '3. Also we need for 5.2 to have the <supported-languages> tag according to the available languages.

    '5.3
    '1. “label” elements requires a xml :lang attribute and not a lang attribute.
    '2. “template-desc” element cannot be translated.

    '5.4, we have a mixed of the issues of 5.2 & 5.3.

    '5.13.xml - “template-desc”

    '6.1.xml – “obj-plan-desc”
    '6.2.xml – “obj-plan-desc”
    '6.5.xml – “obj-plan-desc”

    'Ver 1.1
    'New Class xmlDefinition added.
    'CP tooltip added.
    'Logic updated to compare source for MDF files

    'ver 1.2
    'Fixed
    'When applying partial translations to 5.16, the process fails with various errors. I fixed 1, but I’m giving up now, as there is another one (below).

    'For 5.16, if not all translations are present output just the translations which are present; those which not translated should stay with enUS language code (this is not the case today).
    'And mention an error with the number of missing translations.

    'Ver 1.3 Mega release
    'So many fixes i lost the history itself. But this is a major updated tool now.
    'Project folder structure is changed, added Competency folder, Picklist folder.
    'Added Csv to Xml form
    'Added GUID adder and replacer.

    'Ver 1.4
    'Updated Richtextbox log colors and fixed some issues with Hybris extraction



    'Ver 6.3
    'Updated Error list dock form as we have inVisual IDE.
    'Updated Find function window in Log list
    'Updated DB correction Tools->Corrections->DB correction
    'Added new overall progress bar
    'Added a button to control scrolling of vertical bar

    'Started Working on Support for Length restrictions

    'Ver6.4
    'Refactoring done, created a class CloudProjectsetting and InitiateProcess
    '
    'Ver6.5
    'Form_XliffToXlsConverter, simplified the proecess. Removed the extra 2 buttons

    'Ver6.6
    '

    'Ver6.7
    'Added new function Extract xliff from Input file with translations in 09-Existing translation

    'ver6.8
    'Issue fixed when cleaning piclist, it was considering from 3rd row.

    'ver6.9
    'Added Analyze function - It creates an excel report.
    'Updates New Translation, Wrong Translation, Exact Translation

    'ver7.0
    'Added CP list
    '    Personal Information
    'Last Name
    'First Name
    'Job Information
    'Job Title
    'Address & Contact Information
    'Address Line 1
    'Address Line 2
    'City
    'Country
    'Formal Education(Multiple)
    'School
    'Major
    'Degree
    'Certifications/Licenses (Multiple)
    'Certification/License
    'Description
    'Institution
    'Work Experience within Company (Multiple)
    'Title 
    'Department
    'Previous Employment(Multiple)
    'Company Name
    'Type of Business
    'Title 

    '7.1
    'Added support to create report as txt format, as in server we cannot create report with excel

    '7.2
    'Fixed CP issue faced by Steve

    '7.3
    'Fixed CP issue facdy by steve for personal data where we had \

    '7.4
    'Updated Analyze function - it was stopping because of errors - now i am writin the errors to log and the tool will still continue to process other source

    '7.5
    'Onboarding xml file element format has changed, accordingly i have updated the xliff and xml generattion functions

End Module


