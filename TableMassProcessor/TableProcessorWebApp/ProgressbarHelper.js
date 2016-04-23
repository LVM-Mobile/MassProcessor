        var progress_timer;
        var download_link;

        $(function () {
            
           
            $("#progressbar").progressbar(0);

            submit_button = $('input[type = submit].longoperation');
            if (submit_button.defaultValue = 'Finish') {
                
                progress_timer = setTimeout(updateProgress, 1000);
            }

            submit_button.click(
                        function () {
                            if (submit_button.defaultValue = 'Finish') {

                                progress_timer = setTimeout(updateProgress, 1000);
                               // submit_button.attr('disabled', 'disabled');
                            }
                        });
        });

       function updateProgress() {
           $.getJSON("~/progress.json",
         function (data) {
             /*alert(data.Label);*/
             $("#progressbar").progressbar({ value: data.Value });
             $("#progress_text").text(data.Label);
             if (data.Value < 100) {
                 progress_timer = setTimeout(updateProgress, 1000);
             }
             else {
                 //alert('Process complete');
                 filename = $("#TextBoxOutputFileName").val();
                 SessionID = $("#SessionID").val();
                 download_link = "Download.ashx?fn=" + filename + "&sessionid=" + SessionID;
                 $("#DownloadUrl").attr("href", download_link);

                 window.open(download_link, "_self");
                 window.clearTimeout(progress_timer);
                 submit_button.removeAttr('disabled');
             }
         });
        }