
lendtomeControllers.controller('authController', ['$scope', '$auth', 'SatellizerStorage', '$location', '$rootScope',
  function ($scope, $auth, storage, $location, $rootScope) {

      $scope.signin = function (provider) {
          $auth.authenticate(provider).then(function () {
              var payload = $auth.getPayload();
              var userName = payload.Claims[0].Value;
              storage.set('userName', userName);
              storage.set('userId', payload.Claims[1].Value);
              $rootScope.$broadcast('signedin');
              $location.path('/books');
          });

      };
  }]);

lendtomeControllers.controller('navController', [
    '$scope', '$auth', '$location', 'SatellizerStorage',
    function ($scope, $auth, $location, storage) {

        $scope.$on('signedin', function() {
            $scope.isAuthed = true;
            $scope.userName1 = storage.get('userName');
        });

        $scope.signout = function () {
            $auth.logout();
            $scope.isAuthed = false;
            $scope.userName1 = '';
            $location.path('/');
        }

    }
]);