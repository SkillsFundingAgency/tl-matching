﻿/// <binding BeforeBuild='default' />

var gulp = require('gulp');
var concat = require('gulp-concat');
var minify = require('gulp-minify');
var sass = require('gulp-sass');
var cleanCSS = require('gulp-clean-css');
var concatCss = require('gulp-concat-css');

const paths = require('./gulp/paths.json')
const sassOptions = require('./gulp/sassOptions.js')

gulp.task('default', ['govuk-js', 'copy-js', 'copy-employer-js', 'copy-assets', 'sass', 'merge-css']);


gulp.task('govuk-js', () => {
    gulp.src([
        'node_modules/govuk-frontend/*.js',
        'node_modules/govuk-frontend/vendor/**.js',
        'node_modules/govuk-frontend/components/**/*.js'
    ])
        .pipe(gulp.dest('wwwroot/govuk/javascripts'));
});

gulp.task('copy-js', function () {
    return gulp.src([
        'node_modules/jquery/dist/jquery.min.js',
    ])
        .pipe(concat('all.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs))
});


gulp.task('copy-employer-js', function () {
    return gulp.src([
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.js',
        'Frontend/src/javascripts/employer-search.js'
    ])
        .pipe(concat('employer-search.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs))
});

gulp.task('copy-assets', () => {
    gulp.src('Frontend/src/assets/**/*')
        .pipe(gulp.dest('wwwroot/assets'))
});

gulp.task('sass', () => gulp
    .src(paths.src.default)
    .pipe(sass(sassOptions))
    .pipe(gulp.dest(paths.mid.default)));

gulp.task('merge-css', function () {
    return gulp.src([
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.css',
        "Frontend/src/stylesheets/css/*.css"
    ])
        .pipe(concatCss("main.css"))
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(gulp.dest('wwwroot/stylesheets/'));
})
