/// <binding BeforeBuild='downloadVendors' />
var gulp = require('gulp');
var bower = require('gulp-bower');

// DOWNLOAD VENDOR FILES
gulp.task('downloadVendors', function() {
    return bower({ directory: 'wwwroot/assets/vendor' });
});