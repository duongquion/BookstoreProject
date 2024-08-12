$(document).ready(function () {
    $('#provinces').attr('disabled', true);
    $('#district').attr('disabled', true);
    $('#ward').attr('disabled', true);

    LoadProvinces();
});

function LoadProvinces() {
    $('#provinces').empty();

    $.ajax({
        url: '/Cascade/Provinces',
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                $('#provinces').attr('disabled', false);
                $('#provinces').append('<option>--Chọn Tỉnh/Thành Phố--</option>');
                $('#district').append('<option>--Chọn Quận/Huyện--</option>');
                $('#ward').append('<option>--Chọn Phường/Xã--</option>');
                $.each(response, function (i, data) {
                    $('#provinces').append('<option value=' + data.Code + '>' + data.FullName + '</option>');
                });
            }
            else {
                $('#provinces').attr('disabled', true);
                $('#district').attr('disabled', true);
                $('#ward').attr('disabled', true);
                $('#provinces').append('<option>--Error--</option>');
                $('#district').append('<option>--Error--</option>');
                $('#ward').append('<option>--Error--</option>');
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}