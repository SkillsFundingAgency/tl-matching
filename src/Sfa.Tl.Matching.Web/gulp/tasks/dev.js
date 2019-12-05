/*Reserved for dev build only gulp tasks */

const { src } = require('gulp');

var gulp = require('gulp');
var concat = require('gulp-concat');
var minify = require('gulp-minify');
var sass = require('gulp-sass');
var cleanCSS = require('gulp-clean-css');
var concatCss = require('gulp-concat-css');

const paths = require('../paths.json');
const sassOptions = require('../sassOptions.js');


gulp.task('dev-copy-opportunity-basket-js', function () {
    return src([
        'Frontend/src/javascripts/opportunity-basket.js'
    ])
        .pipe(concat('opportunity-basket.min.js'))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('dev-copy-employer-js', function () {
    return src([
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.js',
        'Frontend/src/javascripts/employer-search.js'
    ])
        .pipe(concat('employer-search.js'))
        .pipe(gulp.dest(paths.dist.defaultJs));
});


gulp.task('dev-copy-editquals-js', function () {
    return src([
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.js',
        'Frontend/src/javascripts/edit-qualifications.js'
    ])
        .pipe(concat('edit-quals.min.js'))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('dev-copy-missing-quals-js', function () {
    return src([
            'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.js',
            'Frontend/src/javascripts/missing-qualification-search.js'
        ])
        .pipe(concat('missing-quals.min.js'))
        .pipe(gulp.dest(paths.dist.defaultJs));
});
