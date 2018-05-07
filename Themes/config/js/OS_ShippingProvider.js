

$(document).ready(function () {

    $('#os_shippingprovider_cmdSave').unbind("click");
    $('#os_shippingprovider_cmdSave').click(function () {
        $('.processing').show();
        $('.actionbuttonwrapper').hide();
        nbxget('os_shippingprovider_savesettings', '.OS_ShippingProviderdata', '.OS_ShippingProviderreturnmsg');
    });

    $('.selectlang').unbind("click");
    $(".selectlang").click(function () {
        $('.editlanguage').hide();
        $('.actionbuttonwrapper').hide();
        $('.processing').show();
        $("#nextlang").val($(this).attr("editlang"));
        nbxget('os_shippingprovider_selectlang', '.OS_ShippingProviderdata', '.OS_ShippingProviderdata');
    });


    $(document).on("nbxgetcompleted", NBS_os_shippingprovider_nbxgetCompleted); // assign a completed event for the ajax calls

    // function to do actions after an ajax call has been made.
    function NBS_os_shippingprovider_nbxgetCompleted(e) {

        $('.processing').hide();
        $('.actionbuttonwrapper').show();
        $('.editlanguage').show();

        if (e.cmd == 'os_shippingprovider_selectlang') {
                        
        }

    };

});

