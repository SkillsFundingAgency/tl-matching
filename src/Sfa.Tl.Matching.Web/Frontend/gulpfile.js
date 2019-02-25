var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var concatCss = require('gulp-concat-css');

gulp.task('default', ['copy-js', 'copy-css', 'copy-assets']);

gulp.task('copy-js', function () {
    return gulp.src([
        'node_modules/jquery/dist/jquery.min.js',
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.js',
        'src/javascripts/*.js'
    ])
        .pipe(concat('javascripts/all.js'))
        .pipe(gulp.dest('../wwwroot/'))
});

gulp.task('copy-css', function () {
    return gulp.src([
        'src/stylesheets/*.css', 
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.css'
    ])
        .pipe(concatCss('stylesheets/all.css'))
        .pipe(gulp.dest('../wwwroot/'))
});


gulp.task('copy-assets', () => {
    gulp.src('src/assets/**/*')
        .pipe(gulp.dest('../wwwroot/assets'))
});

