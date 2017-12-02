ko.components.register('qr-scanner', {
    viewModel: function (params) {
        var self = this;

        self.types = {
            Award: {
                Name: 'Award',
                Endpoint: '/award/logaction'
            },
            Redemption: {
                Name: 'Redemption',
                Endpoint: '/redemption/logaction'
            },
            Balance: {
                Name: 'Balance',
                Endpoint: '/user/checkbalance'
            }
        }

        self.state = {
            type: null,
            error: ko.observable(null),
            success: ko.observable(null),
            currentCamera: ko.observable(null),
            scannerActive: ko.observable(false),
            loading: ko.observable(false),
            awardConfirmation: ko.observable(false),
            redemptionConfirmation: ko.observable(false),
            balanceConfirmation: ko.observable(false)
        }

        self.data = {
            scanner: ko.observable(null),
            cameras: ko.observableArray([]),
            code: ko.observable(null),
            awardNumber: null,
            redemptionNumber: null,
            awardConfirmation: {
                pointsBalance: ko.observable(0),
                pointsAwarded: ko.observable(0),
                userNumber: ko.observable(null)
            },
            redemptionConfirmation: {
                success: ko.observable(false),
                pointsBalance: ko.observable(0),
                pointsShort: ko.observable(0),
                userNumber: ko.observable(null)
            },
            balanceConfirmation: {
                success: ko.observable(false),
                pointsBalance: ko.observable(0),
                userNumber: ko.observable(null)
            },
            externalID: ko.observable(null)
        };

        self.events = {
            onScan: function (content) {
                
                if (content) {
                    self.data.code(content)
                    self.methods.submit();
                }
            },
            onActive: function () {
                self.state.scannerActive(true);
            },
            onInActive: function () {
                self.state.scannerActive(false);
            },
            onCamerasLoaded: self.data.cameras.subscribe(function (cameras) {
                if (cameras.length > 0) {
                    self.state.currentCamera(cameras[0]);
                    
                } else {
                    self.state.error('No cameras found.');
                }
            }),
            onCameraSelected: self.state.currentCamera.subscribe(function (camera) {
                self.data.scanner().start(camera);
            })
        }

        self.methods = new function (params) {
            var m = this;

            m.init = function (params) {
                if (params.redemptionNumber) {
                    self.state.type = self.types.Redemption;
                    self.data.redemptionNumber = params.redemptionNumber;
                }
                if (params.awardNumber) {
                    self.state.type = self.types.Award;
                    self.data.awardNumber = params.awardNumber;
                }

                if (!params.redemptionNumber && !params.awardNumber) {
                    self.state.type = self.types.Balance;
                }
            };

            m.changeCamera = function (camera) {
                self.state.currentCamera(camera);
            }


            m.submit = function () {

                if (self.state.type === self.types.Award) {
                    m.submitAward();
                }

                if (self.state.type === self.types.Redemption) {
                    m.submitRedemption();
                }

                if (self.state.type === self.types.Balance) {
                    m.submitCheckBalance();
                }
            };

            m.submitAward = function () {
                var postData = {
                    UserNumber: self.data.code(),
                    AwardNumber: self.data.awardNumber
                };

                self.methods.dismissError();
                self.methods.dismissConfirmation();
                self.state.loading(true);

                $.post(self.state.type.Endpoint, postData)
                    .done(function (response) {

                        if (response.errorMessage) {
                            m.showError(response.errorMessage);
                        } else {
                            m.showAwardConfirmation(response.responseData);
                        }

                    }).fail(function (error) {
                        //error
                        m.showError(error);
                    }).always(function () {
                        //always
                        self.state.loading(false);
                    });
            }

            m.submitRedemption = function () {
                var postData = {
                    UserNumber: self.data.code(),
                    RedemptionNumber: self.data.redemptionNumber
                };

                self.methods.dismissError();
                self.methods.dismissConfirmation();
                self.state.loading(true);

                $.post(self.state.type.Endpoint, postData)
                    .done(function (response) {

                        if (response.errorMessage) {
                            m.showError(response.errorMessage);
                        } else {
                            m.showRedemptionConfirmation(response.responseData);
                        }

                    }).fail(function (error) {
                        //error
                        m.showError(error);
                    }).always(function () {
                        //always
                        self.state.loading(false);
                    });
            }

            m.submitCheckBalance = function () {
                var postData = {
                    UserNumber: self.data.code()
                };

                self.methods.dismissError();
                self.methods.dismissConfirmation();
                self.state.loading(true);

                $.post(self.state.type.Endpoint, postData)
                    .done(function (response) {

                        if (response.errorMessage) {
                            m.showError(response.errorMessage);
                        } else {
                            m.showBalanceConfirmation(response.responseData);
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
                self.data.code(null);
            };

            m.dismissError = function () {
                self.state.error(null);
            };

            m.showAwardConfirmation = function (data) {
                self.data.code(null);
                self.data.externalID(data.externalID);
                self.data.awardConfirmation.pointsAwarded(data.pointsAwarded);
                self.data.awardConfirmation.pointsBalance(data.pointsBalance);
                self.data.awardConfirmation.userNumber(data.userNumber);
                self.state.awardConfirmation(true);
            };

            m.showRedemptionConfirmation = function (data) {
                self.data.code(null);
                self.data.externalID(data.externalID);
                self.data.redemptionConfirmation.success(data.success);
                self.data.redemptionConfirmation.pointsShort(data.pointsShort);
                self.data.redemptionConfirmation.pointsBalance(data.pointsBalance);
                self.data.redemptionConfirmation.userNumber(data.userNumber);
                self.state.redemptionConfirmation(true);
            };

            m.showBalanceConfirmation = function (data) {
                self.data.code(null);
                self.data.externalID(data.externalID);
                self.data.balanceConfirmation.success(data.success);
                self.data.balanceConfirmation.pointsBalance(data.pointsBalance);
                self.data.balanceConfirmation.userNumber(data.userNumber);
                self.state.balanceConfirmation(true);
            };

            m.dismissConfirmation = function () {
                self.state.awardConfirmation(false);
                self.data.awardConfirmation.pointsAwarded(0);
                self.data.awardConfirmation.pointsBalance(0);
                self.data.awardConfirmation.userNumber(null);

                self.state.redemptionConfirmation(false);
                self.data.redemptionConfirmation.success(false);
                self.data.redemptionConfirmation.pointsShort(0);
                self.data.redemptionConfirmation.pointsBalance(0);
                self.data.redemptionConfirmation.userNumber(null);

                self.state.balanceConfirmation(false);
                self.data.balanceConfirmation.success(false);
                self.data.balanceConfirmation.pointsBalance(0);
                self.data.balanceConfirmation.userNumber(null);

                self.data.externalID(null);
            };
        };

        self.methods.init(params);
    },
    template: {
        element: 'qr-scanner-tmpl'
    }
});