import React, { Component } from 'react';
import UtcDate from '../common/UtcDate'

class ListPageLink extends Component {

  render() {

    let label = null;
    if (this.props.grouped)
    	label = <p/>;
    else if (this.props.suiteEndTime)
    	label = <p className="text-center text-muted"><small>{new UtcDate(this.props.suiteEndTime).getTimeFromNowWithSuffix()}</small></p>;
    else
    	label = <p className="text-center text-warning"><small>running...</small></p>;

    return (
      label
    );
  }
}

export default ListPageLink;
