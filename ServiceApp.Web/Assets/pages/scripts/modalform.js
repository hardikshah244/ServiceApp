$(function () {

    $.ajaxSetup({ cache: false });

    //show.bs.modal
    $('a[data-modal]').on('click', function (e) {

        // hide dropdown if any
        // $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');
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
        var formdata = new FormData($('form').get(0));
        $.ajax({
            url: this.action,
            type: this.method,
            data: formdata,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result.success) {
                    alert(result.Message);
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