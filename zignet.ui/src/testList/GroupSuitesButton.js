import React, { Component } from 'react';

class GroupSuitesButton extends Component {

  render() {
  	const suiteId = this.props.suiteId;
    const button = this.props.grouped ? 
      <a href={"/" + suiteId} className="btn btn-outline-primary float-right" role="button" title="Click to ungroup results">Ungroup</a> :
      <a href={"/" + suiteId + "?group=true"} className="btn btn-primary float-right" role="button" title="Click to group tests by environment">Group</a>;

    return (
      button
    );
  }
}

export default GroupSuitesButton;
