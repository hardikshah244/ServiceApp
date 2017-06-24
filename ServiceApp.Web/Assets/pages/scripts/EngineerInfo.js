$(function () { // will trigger when the document is ready

    $('.datepicker').datepicker({
        format: 'dd-mm-yyyy'
    })
   .on('changeDate', function (e) {
       var oldDate = new Date(e.date);
       var newDate = new Date();
       newDate.setDate(oldDate.getDate() + 30);

       $("#EndDate").val(('0' + newDate.getDate()).slice(-2) + "-" + ('0' + (newDate.getMonth() + 1)).slice(-2) + "-" + newDate.getFullYear());
   });

    if ($("#MembershipType option:selected").val() == "Paid") {
        $("#dvStartDate").css("display", "block");
        $("#dvEndDate").css("display", "block");
        $("#dvAmount").css("display", "block");
    }

    $("#MembershipType").change(function () {
        var MembershipType = $("#MembershipType option:selected").val();
        if (MembershipType == "Paid") {
            $("#dvStartDate").css("display", "block");
            $("#dvEndDate").css("display", "block");
            $("#dvAmount").css("display", "block");
        } else {
            $("#StartDate").val("");
            $("#EndDate").val("");
            $("#Amount").val("");

            $("#dvStartDate").css("display", "none");
            $("#dvEndDate").css("display", "none");
            $("#dvAmount").css("display", "none");
        }
    });

    $("#ImagePath").change(function () {
        var fileExtension = ['jpeg', 'jpg', 'png', 'pdf', 'doc', 'docx', 'zip'];

        for (var i = 0; i < $(this).get(0).files.length; ++i) {
            if ($.inArray($(this).get(0).files[i].name.split('.').pop().toLowerCase(), fileExtension) == -1) {
                alert("Only formats are allowed : " + fileExtension.join(', '));
                $("#ImagePath").val(null);
                break;
            }
        }
    });

    $('.DeleteDoc').click(function (e) {
        e.preventDefault();
        var $ctrl = $(this);
        if (confirm('Do you really want to delete this file?')) {
            $.ajax({
                url: '/EngineerInfo/DeleteDoc',
                type: 'POST',
                data: { Doc: $(this).attr("data-Doc"), FileName: $(this).attr("data-FileName") }
            }).done(function (data) {
                if (data.Result == "OK") {
                    $ctrl.closest('tr').remove();

                    //Remove file from hidden field
                    var strFileDetails = $("#FileDetails").val();
                    var strVal = removeValue(strFileDetails, $(this).attr("data-FileName"));
                    if (strVal = "")
                        $("#FileDetails").val(null);
                    else
                        $("#FileDetails").val(strVal);
                }
                else if (data.Result.Message) {
                    alert(data.Result.Message);
                }
            }).fail(function () {
                alert("There is something wrong. Please try again.");
            })
        }
    });

    function removeValue(list, value) {
        list = list.split(',');
        list.splice(list.indexOf(value), 1);
        return list.join(',');
    }

});
