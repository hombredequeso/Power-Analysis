window.HDQ.namespace('HDQ.maccurve');

window.HDQ.maccurve = (function () {
	return {
		renderChart: function (vm, ctx) {


			var line = function (a, b) {
				ctx.beginPath();
				ctx.moveTo(a.x, a.y);
				ctx.lineTo(b.x, b.y);
				ctx.stroke();
			};
			var canvasXOffset = 50;


			var items = ko.mapping.toJS(vm.items);


			var currentX = 0;
			var preProcess = function (e) {
				var result = $.extend({ }, e, {
					x: currentX,
					y: 0,
					width: e.xCalc,
					height: e.yCalc
				});
				currentX += e.xCalc;
				return result;
			};

			var preProcessedItems = items.map(preProcess);
			preProcessedItems.sort(function (a, b) { return a.height - b.height; });
			var xInItemCoords = currentX;
			var canvasX = 500;
			var canvasRightBorder = 50;
			var canvasXForItems = canvasX - canvasXOffset - canvasRightBorder;
			var xScaling = canvasXForItems / xInItemCoords;

			var yCoords = items.map(function (i) { return i.yCalc; });
			var yMaxInItemCoords = Math.max.apply(Math, yCoords);
			var yMinInItemCoords = Math.min.apply(Math, yCoords);
			var yRange = yMaxInItemCoords - yMinInItemCoords;


			var canvasY = 500;
			var topBorder = 10, bottomBorder = 10, spaceForLabels = 20;
			var yVpSpace = canvasY - topBorder - bottomBorder - (2 * spaceForLabels);
			var yScaling = yVpSpace / yRange;

			var chartValueToViewPortTransform = function (p) {
				return $.extend({ }, p, {
					x: p.x * xScaling,
					y: p.y * -1 * yScaling,
					width: p.width * xScaling,
					height: p.height * -1 * yScaling
				});
			};

			var itemsInVpCoords = preProcessedItems.map(chartValueToViewPortTransform);

			var drawItem = function (i) {
				ctx.fillRect(i.x, i.y, i.width, i.height);
				ctx.strokeRect(i.x, i.y, i.width, i.height);
			};

			var drawLabel = function (i) {
				var endPos = i.lineEndPos;
				line(i.lineStartPos, i.lineEndPos);
				if (endPos.y > 0) {
					ctx.textAlign = 'left';
					ctx.textBaseline = 'middle';
					ctx.fillText(i.label, endPos.x + lineToLabel, endPos.y);
				} else {
					ctx.textAlign = 'right';
					ctx.textBaseline = 'middle';
					ctx.fillText(i.label, endPos.x - lineToLabel, endPos.y);
				}
			};

			// split + and -
			var count = itemsInVpCoords.length;
			var currentPos = 0, breakPos = 0;
			while (itemsInVpCoords[currentPos].height > 0 && currentPos < count) {
				++breakPos;
				++currentPos;
			}
			var positiveItems = itemsInVpCoords.slice(0, breakPos);
			var negItems = itemsInVpCoords.slice(breakPos);

			var labelLineLength = 20;
			var lineToLabel = 2;
			var minLabelDistance = 20;
			var lastLabel = 0;
			var calculateLabelPos = function (i) {
				var midPointX = i.x + (i.width / 2);
				i.lineStartPos = { x: midPointX, y: i.height };
				i.lineEndPos = {
					x: midPointX,
					y: i.height > 0 ? i.height + labelLineLength : i.height - labelLineLength
				};
				if (lastLabel + minLabelDistance > i.lineEndPos.y) {
					i.lineEndPos.y = lastLabel + minLabelDistance;
				}
				lastLabel = i.lineEndPos.y;
			};

			positiveItems.reverse();
			positiveItems.forEach(calculateLabelPos);

			var calculateNegLabelPos = function (i) {
				var midPointX = i.x + (i.width / 2);
				i.lineStartPos = { x: midPointX, y: i.height };
				i.lineEndPos = {
					x: midPointX,
					y: i.height - labelLineLength
				};
				if (lastLabel - minLabelDistance < i.lineEndPos.y) {
					i.lineEndPos.y = lastLabel - minLabelDistance;
				}
				lastLabel = i.lineEndPos.y;
			};
			lastLabel = 0;
			negItems.forEach(calculateNegLabelPos);

			ctx.clearRect(0, 0, canvasX, canvasY);
			ctx.save();

			var yTranslation = (yMaxInItemCoords * yScaling) + topBorder + spaceForLabels;
			ctx.translate(canvasXOffset, yTranslation);

			// Draw axis
			line({ x: 0, y: -yTranslation + 7 }, { x: 0, y: (canvasY - yTranslation) });
			var horizontalAxisOverhang = 10;
			line({ x: 0, y: 0 }, { x: 450 - canvasRightBorder + horizontalAxisOverhang, y: 0 });
			
			// axis label
			// x : Mt CO2
			ctx.fillText('$/t CO2', -15, -yTranslation + 7);
			// y : $/t CO2
			ctx.save();
			ctx.textBaseline = 'middle';
			ctx.fillText('Mt CO2', 450 - canvasRightBorder + horizontalAxisOverhang + 5, 0);
			ctx.restore();
			
			var getInterval = function (x) {
				var intervals = [0.1, 0.5, 1, 5, 10, 20, 50, 100, 500, 1000, 5000, 10000];
				var l = intervals.length;
				for (var i = 0; i < l; i++) {
					if (x <= intervals[i]) return intervals[i];	
				}
				return 100000;
			};
			
			// vertical scaling
			ctx.save();
			ctx.textBaseline = 'middle';
			ctx.textAlign = 'right';
//			ctx.fillText('0', -7, 0);
//			line({x:0,y:0}, {x: -5,y: 0});
			var interval = getInterval(Math.floor(yRange / 10));
			var currentPos = 0;
			while (currentPos <= yMaxInItemCoords) {
				ctx.fillText(currentPos.toString(), -7, -currentPos * yScaling);
				line({x:0,y:-currentPos * yScaling}, {x: -5,y: -currentPos * yScaling});
				currentPos += interval;
			}

			var currentPos = -interval;
			while (currentPos > yMinInItemCoords) {
				ctx.fillText(currentPos.toString(), -7, -currentPos * yScaling);
				line({x:0,y:-currentPos * yScaling}, {x: -5,y: -currentPos * yScaling});
				currentPos -= interval;
			}
			ctx.restore();


			ctx.fillStyle = 'green';
			ctx.strokeStyle = 'black';
			itemsInVpCoords.forEach(drawItem);
			ctx.fillStyle = 'black';
			itemsInVpCoords.forEach(drawLabel);
			
			// horizontal scale
			ctx.save();
			ctx.textBaseline = 'middle';
			ctx.textAlign = 'center';
			interval = getInterval(xInItemCoords/10);
			currentPos = 0 + interval;
			while (currentPos <= xInItemCoords) {
				ctx.fillText(currentPos.toString(), currentPos * xScaling, 10);
				line({x:currentPos * xScaling,y:0}, {x: currentPos * xScaling,y: 5});
				currentPos += interval;
			}
			ctx.restore();


			ctx.restore();
		}
	};
}());