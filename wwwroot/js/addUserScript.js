$(document).ready(function () {
    $('#registerForm').validate({
        normalizer: function (value) {
            return $.trim(value);
        },
        rules: {
            "fullname": {
                required: true,
                minlength: 5,
                maxlength: 50
            },
            "phone": {
                required: true,
                digits: true,
                minlength: 9,
                maxlength: 13
            },
            "address": {
                required: true,
                minlength: 5,
                maxlength: 50
            }
        },
        messages: {
            "fullname": {
                required: "Vui lòng nhập họ và tên khách hàng",
                minlength: "Họ và tên phải có ít nhất 5 ký tự",
                maxlength: "Họ và tên không được vượt quá 50 ký tự"
            },
            "phone": {
                required: "Vui lòng nhập số điện thoại",
                digits: "Vui lòng chỉ nhập số",
                minlength: "Số điện thoại phải có ít nhất 9 ký tự",
                maxlength: "Số điện thoại không được vượt quá 13 ký tự"
            },
            "address": {
                required: "Vui lòng nhập địa chỉ",
                minlength: "Địa chỉ phải có ít nhất 5 ký tự",
                maxlength: "Địa chỉ không được vượt quá 50 ký tự"
            }
        },
        submitHandler: function (form, event) {
            form.submit();
        }
    });
});