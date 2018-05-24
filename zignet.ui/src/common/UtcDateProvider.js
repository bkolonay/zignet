import moment from 'moment';

function getTimeFromNowWithSuffix(utcDateTime) {
	let localMoment = moment.utc(utcDateTime);
	return localMoment.fromNow();
}

export { getTimeFromNowWithSuffix }