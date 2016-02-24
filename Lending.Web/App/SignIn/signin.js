
lendtomeControllers.controller('signinController', ['$scope', '$auth', 'SatellizerStorage', '$location', '$rootScope',
  function ($scope, $auth, storage, $location, $rootScope) {

      $scope.authenticate = function (provider) {
          $auth.authenticate(provider).then(function () {
              var payload = $auth.getPayload();
              var userName = payload.Claims[0].Value;
              storage.set('userName', userName);
              storage.set('userId', payload.Claims[1].Value);
              $rootScope.isAuthenticated = true;
              $rootScope.userName = userName;
              $location.path('/books');
          });
      };
  }]);