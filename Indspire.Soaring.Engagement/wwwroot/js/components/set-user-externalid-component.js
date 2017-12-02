ko.components.register('set-user-externalid', {
    viewModel: function (params) {
        var self = this;

        self.state = {
            showForm: ko.observable(false),
            error: ko.observable(null),
            success: ko.observable(null),
            loading: ko.observable(false),
            timeout: null
        }

        self.data = {
            userNumber: ko.observable(null),
            externalID: ko.observable(null)
        };


        self.methods = new function (params) {
            var m = this;

            m.init = function (params) {
                self.data.userNumber = params.userNumber;
                self.data.externalID = params.externalID;
                
                //have an externalID already? kickout
                if (self.data.externalID() && self.data.externalID().length > 0) {
                    return;
                }

                self.state.showForm(true);
            };

            m.submit = function () {

                var postData = {
                    userNumber: self.data.userNumber(),
                    externalID: self.data.externalID()
                }

                self.state.loading(true);

                $.post('/user/setexternalid', postData)
                    .done(function (response) {

                        if (response.errorMessage) {
                            m.showError(response.errorMessage);
                        } else {
                            m.showConfirmation(response.responseData);
                        }

                    }).fail(function (error) {
                        //error
                        m.showError(error);
                    }).always(function () {
                        //always
                        self.state.loading(false);
                    });
            }

            
            m.showError = function (errorMsg) {
                self.state.error(errorMsg);
            };

            m.showConfirmation = function (data) {
                self.state.showForm(false);
                self.state.success('Success. ExternalID has been set.');

                self.timeout = setTimeout(function () {
                    self.state.success(null);
                }, 3000)
            };

        };

        self.dispose = function () {
            if ($.isFunction(self.state.timeout)) {
                clearTimeout(self.state.timeout);
            }

            self.state.error(null);
            self.state.success(null)
        }

        self.methods.init(params);
    },
    template: {
        element: 'set-user-externalid-tmpl'
    }
});