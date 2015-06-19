var gulp = require("gulp");
var plugins = require("gulp-load-plugins")();

gulp.task("clean", require("rimraf").bind(null, "dist"));

gulp.task("dtsm", function () {
    return gulp.src("dtsm.json").pipe(plugins.dtsm());
});

gulp.task("default", ["clean", "dtsm"], function () {
    var options = require("./tsconfig.json").compilerOptions;
    options.typescript = require("typescript");
    return gulp.src("src/**/*.ts")
        .pipe(plugins.typescript(options))
        .js.pipe(gulp.dest("dist"));
});
