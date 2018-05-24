import React, { Component } from 'react';

class ListPageLink extends Component {

  render() {

	  const link = this.props.grouped ? 
	  	<a href={`/${this.props.suiteId}?group=true`}>Total: {this.props.totalTests}</a> :
	  	<a href={`/${this.props.suiteId}`}>Total: {this.props.totalTests}</a>;

    return (
      link
    );
  }
}

export default ListPageLink;
