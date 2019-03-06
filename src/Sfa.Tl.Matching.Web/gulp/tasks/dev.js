
var gulp = require('gulp');
var concat = require('gulp-concat');
var minify = require('gulp-minify');
var sass = require('gulp-sass');
var cleanCSS = require('gulp-clean-css');
var concatCss = require('gulp-concat-css');

const paths = require('../paths.json')
const sassOptions = require('../sassOptions.js')


gulp.task('dev-govuk-js', () => {
    gulp.src([
        'node_modules/govuk-frontend/*.js',
        'node_modules/govuk-frontend/vendor/**.js',
        'node_modules/govuk-frontend/components/**/*.js'
    ])
        .pipe(gulp.dest('wwwroot/govuk/javascripts'));
});

gulp.task('dev-copy-js', function () {
    return gulp.src([
        'node_modules/jquery/dist/jquery.min.js',
    ])
        .pipe(concat('all.js'))
        .pipe(gulp.dest(paths.dist.defaultJs))
});


gulp.task('dev-copy-employer-js', function () {
    return gulp.src([
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.js',
        'Frontend/src/javascripts/employer-search.js'
    ])
        .pipe(concat('employer-search.min.js'))
        .pipe(gulp.dest(paths.dist.defaultJs))
});

gulp.task('dev-copy-assets', () => {
    gulp.src('Frontend/src/assets/**/*')
        .pipe(gulp.dest('wwwroot/assets'))
});

gulp.task('dev-sass', () => gulp
    .src(paths.src.default)
    .pipe(sass(sassOptions))
    .pipe(gulp.dest(paths.mid.default)));

gulp.task('dev-merge-css', function () {
    return gulp.src([
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.css',
        "Frontend/src/stylesheets/css/*.css"
    ])
        .pipe(concatCss("main.css"))
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(gulp.dest('wwwroot/stylesheets/'));
})
