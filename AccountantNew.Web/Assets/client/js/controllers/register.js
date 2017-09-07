var register = {
    init: function () {
        register.registerEvent();
    },
    registerEvent: function () {
        $('#frmRegister').validate({
            rules: {
                UserName: "required",
                FullName: "required",
                Email: {
                    required: true,
                    email: true
                },
                PhoneNumber: {
                    required: true,
                    number: true
                },
                Image: {
                    required: true,
                    extension: "jpg|png"
                }
            },
            messages: {
                UserName:"Yêu cầu nhập vào user domain",
                FullName: "Yêu cầu nhập tên",
                Email: {
                    required: "Yêu cầu nhập email",
                    email: "Định dạng email chưa đúng"
                },
                PhoneNumber: {
                    required: "Yêu cầu nhập số điện thoại",
                    number: "Số điện thoại phải là số."
                },
                Image: "Yêu cầu chọn hình",
            }
        });
    }
}

register.init();