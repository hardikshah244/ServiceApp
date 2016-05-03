$(function () {

    $.ajaxSetup({ cache: false });

    $("a[data-modal]").on("click", function (e) {

        // hide dropdown if any
        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');


        $('#dvModalContent').load(this.href, function () {


            $('#dvModal').modal({
                /*backdrop: 'static',*/
                keyboard: false
            }, 'show');

            bindForm(this);
        });

        return false;
    });


});

function bindForm(dialog) {

    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#dvModal').modal('hide');
                    //Refresh
                    location.reload();
                } else {
                    $('#dvModalContent').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}